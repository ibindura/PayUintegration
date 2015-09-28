using System.Collections.Generic;

namespace Bindsoft.PayuIntergration
{
	public class DoTransactionReqest
	{
		public string Api { get; set; }
		public string SafeKey { get; set; }
		public string CancelUrl { get; set; }
		public string OrderNumber { get; set; }
		public string ReturnUrl { get; set; }
		public string PaymentTypes { get; set; }
		public string CustomerName { get; set; }
		public string CustomerLastname { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerMobile { get; set; }
		public string CustomerUsername { get; set; }
		public string AmountInCents { get; set; }
		public List<LineItem> LineItems { get; set; }
		public string PayUReference { get; set; }
	}
}