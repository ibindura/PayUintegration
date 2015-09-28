namespace Bindsoft.PayuIntergration
{
	public class SetTransactionResult
	{
		public bool IsTransactionSet { get; set; }
		public string MerchantReference { get; set; }
		public string PayUReference { get; set; }
		public string DisplayMessage { get; set; }
		public string ErrorMessage { get; set; }
		public string ResultCode { get; set; }
		public string RequestTrace { get; set; }
	}
}