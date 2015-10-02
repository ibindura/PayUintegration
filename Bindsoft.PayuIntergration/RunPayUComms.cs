using System;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Bindsoft.PayuIntergration.PayUWcf;

namespace Bindsoft.PayuIntergration
{
	public class RunPayUComms
	{
		private readonly string _safeKey;
		private readonly string _api;

		private readonly EnterpriseAPISoapClient _client;
		public RunPayUComms(string username, string password, string apiurl, string safeKey, string api)
		{
			_safeKey = safeKey;
			_api = api;

			var binding = new BasicHttpBinding
			{
				Name = "EnterpriseAPISoapServiceSoapBinding1",
				CloseTimeout = new TimeSpan(0, 0, 1, 0),
				OpenTimeout = new TimeSpan(0, 0, 1, 0),
				ReceiveTimeout = new TimeSpan(0, 0, 10, 0),
				SendTimeout = new TimeSpan(0, 0, 10, 0),
				AllowCookies = false,
				BypassProxyOnLocal = false,
				HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
				MaxBufferSize = 65536,
				MessageEncoding = WSMessageEncoding.Text,
				TextEncoding = Encoding.UTF8,
				TransferMode = TransferMode.Buffered,
				UseDefaultWebProxy = true,
				Security = new BasicHttpSecurity
				{
					Message = new BasicHttpMessageSecurity
					{
						ClientCredentialType = BasicHttpMessageCredentialType.UserName,
					},
					Transport = new HttpTransportSecurity
					{
						ClientCredentialType = HttpClientCredentialType.None,
						ProxyCredentialType = HttpProxyCredentialType.None,
					},
					Mode = BasicHttpSecurityMode.Transport
				},
				ReaderQuotas = new XmlDictionaryReaderQuotas
				{
					MaxDepth = 32,
					MaxStringContentLength = 8192,
					MaxArrayLength = 16384,
					MaxBytesPerRead = 4096,
					MaxNameTableCharCount = 16384
				}
			};
			var address = new EndpointAddress(apiurl);
			_client = new EnterpriseAPISoapClient(binding,address);
			_client.Endpoint.Behaviors.Add(new InspectorBehavior(new ClientInspector(new SecurityHeader(username, password))));

		}

		public SetTransactionResult SetTransaction(SetTransactionRequest request)
		{
			try
			{
				var result = _client.setTransaction(
					_api,
					_safeKey,
					transactionType.RESERVE,
					request.Stage,
					new additionalInfo
					{
						cancelUrl = request.CancelUrl,
						merchantReference = request.OrderNumber,
						returnUrl = request.ReturnUrl,
						supportedPaymentMethods = request.PaymentTypes ?? "CREDITCARD,WALLET,DISCOVERYMILES,DEBITCARD"
					}, new customer
					{
						firstName = request.CustomerName,
						lastName = request.CustomerLastname,
						email = request.CustomerEmail,
						mobile = request.CustomerMobile,
						merchantUserId = request.CustomerUsername
					},
					new basket
					{
						amountInCents = request.AmountInCents,
						currencyCode = "ZAR",
						description = request.OrderNumber,
						productLineItem = request.LineItems.Select(x => new productLineItem
						{
							amount = x.Amount,
							description = x.Description,
							productCode = x.ProductCode,
							quantity = x.Quantity
						}).ToArray()
					}, null, null, null, null, null, null, null, null, null, null, null, null
					);
				return new SetTransactionResult
				{
					IsTransactionSet = result.successful,
					MerchantReference = result.merchantReference,
					PayUReference = result.payUReference,
					DisplayMessage = result.displayMessage,
					ErrorMessage = result.resultMessage,
					ResultCode = result.resultCode,
					RequestTrace = result.requestTrace
				};
			}
			catch (Exception ex)
			{
				return new SetTransactionResult
				{
					IsTransactionSet = false,
					DisplayMessage = "An Error Occurred while processing your transaction",
					ErrorMessage = ex.Message
				};
			}


		}

		public GetTransactionResult GetTransaction(GetTransactionRequest request)
		{
			try
			{
				var result = _client.getTransaction(_api, _safeKey, new additionalInfo
					{
						payUReference = request.PayUReference
					});
				return new GetTransactionResult
				{
					IsTransactionSuccessfull = result.successful,
					TransactionState = result.transactionStateSpecified ? result.transactionState.ToString() : "Unknown",
					DisplayMessage = result.displayMessage,
					ErrorMessage = result.resultMessage,
					PayUReference = result.payUReference,
					RequestTrace = result.requestTrace,
					ResultCode = result.resultCode
				};
			}
			catch (Exception ex)
			{

				return new GetTransactionResult
				{
					IsTransactionSuccessfull = false,
					DisplayMessage = "An Error Occurred while processing your transaction",
					ErrorMessage = ex.Message
				};
			}
		}

		public DoTransactionResult DoTransaction(DoTransactionReqest request)
		{
			try
			{
				var result = _client.doTransaction(
					_api,
					_safeKey,
					transactionType.FINALIZE,
					authenticationType.TOKEN,
					new additionalInfo
					{
						merchantReference = request.OrderNumber,
						payUReference = request.PayUReference
					},
					new customer
					{
						firstName = request.CustomerName,
						lastName = request.CustomerLastname,
						email = request.CustomerEmail,
						mobile = request.CustomerMobile,
						merchantUserId = request.CustomerUsername
					},
					new basket
					{
						amountInCents = request.AmountInCents,
						currencyCode = "ZAR",
						description = request.OrderNumber,
						productLineItem = request.LineItems.Select(x => new productLineItem
						{
							amount = x.Amount,
							description = x.Description,
							productCode = x.ProductCode,
							quantity = x.Quantity
						}).ToArray()
					}, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
					null, null
					);
				return new DoTransactionResult
				{
					MerchantReference = result.merchantReference,
					ResultCode = result.resultCode,
					ResultMessage = result.displayMessage,
					Successful = result.successful,
					PayUReference = result.payUReference
				};

			}
			catch (Exception)
			{

				return new DoTransactionResult
				{
					ResultMessage = "An Error Occurred while processing Do Transaction",
					Successful = false
				};
			}
		}
	}
}
