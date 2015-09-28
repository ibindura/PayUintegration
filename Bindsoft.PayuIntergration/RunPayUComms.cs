using System;
using System.Linq;
using System.ServiceModel;
using Bindsoft.PayuIntergration.PayUWcf;

namespace Bindsoft.PayuIntergration
{
	public class RunPayUComms
	{
		private readonly string _safeKey;
		private readonly string _api;

		private readonly EnterpriseAPISoapClient _client;
		public RunPayUComms(string username, string password, string apiurl, string safeKey,string api)
		{
			_safeKey = safeKey;
			_api = api;
			_client = new EnterpriseAPISoapClient();

			_client.Endpoint.Behaviors.Add(new InspectorBehavior(new ClientInspector(new SecurityHeader(username, password))));
			_client.Endpoint.Address = new EndpointAddress(apiurl);

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
