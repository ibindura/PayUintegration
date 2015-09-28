using System;
using System.Collections.Generic;
using Bindsoft.PayuIntergration;
using NUnit.Framework;

namespace PayuFunctionalTest
{
	[TestFixture]
	public class PayUCommsTests
	{
		private RunPayUComms _comms;

		[SetUp]
		public void TestSetup()
		{
			_comms = new RunPayUComms("100032", "PypWWegU", "https://staging.payu.co.za/service/PayUAPI?wsdl", "{CE62CE80-0EFD-4035-87C1-8824C5C46E7F}", "ONE_ZERO");

		}
		[Test]
		public void SetTransaction()
		{
			var result = _comms.SetTransaction(new SetTransactionRequest
			{
				AmountInCents = "1000000",
				CancelUrl = "http://127.0.0.1",
				CustomerEmail = "payu@a-b.co.za",
				CustomerLastname = "Lastname",
				CustomerMobile = "27834812729",
				CustomerName = "Linocent",
				CustomerUsername = "lbindura",
				LineItems = new List<LineItem>
				{
					new LineItem
					{
						Amount = "250000",
						Description = "Shoes kyc",
						ProductCode = "KYS Gh5",
						Quantity = "2"
					},
					new LineItem
					{
						Amount = "500000",
						Description = "Nespresto Coffee machine",
						ProductCode = "Nesp 1300",
						Quantity = "1"
					}
				},
				OrderNumber = "142132",
				PaymentTypes = null,
				ReturnUrl = "http://127.0.0.1",
				Stage = false
			});
			Console.Write("{0} ###### {1}", result.PayUReference, result.ErrorMessage);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.IsTransactionSet);
			Assert.IsNotNull(result.PayUReference);
		}


		[Test]
		public void TestGetTransaction()
		{
			var result = _comms.GetTransaction(new GetTransactionRequest
			{
				PayUReference = "619345383187",
				OrderNumber = "142132"
			});
			Assert.IsNotNull(result);
			Assert.IsTrue(result.IsTransactionSuccessfull);
		}

		[Test]
		public void TestDoTransaction()
		{
			var result = _comms.DoTransaction(new DoTransactionReqest
			{
				AmountInCents = "1000000",
				CancelUrl = "http://127.0.0.1",
				CustomerEmail = "payu@a-b.co.za",
				CustomerLastname = "Lastname",
				CustomerMobile = "27834812729",
				CustomerName = "Linocent",
				CustomerUsername = "lbindura",
				LineItems = new List<LineItem>
				{
					new LineItem
					{
						Amount = "250000",
						Description = "Shoes kyc",
						ProductCode = "KYS Gh5",
						Quantity = "2"
					},
					new LineItem
					{
						Amount = "500000",
						Description = "Nespresto Coffee machine",
						ProductCode = "Nesp 1300",
						Quantity = "1"
					}
				},
				OrderNumber = "142132",
				PaymentTypes = null,
				ReturnUrl = "http://127.0.0.1",
				PayUReference = "619345383187"
			});
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Successful);
			
			Console.WriteLine(result.ResultCode);
		}
	}
}
