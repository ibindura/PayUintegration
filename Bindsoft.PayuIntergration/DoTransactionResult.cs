namespace Bindsoft.PayuIntergration
{
	public class DoTransactionResult
	{
		public string MerchantReference { get; set; }
		public string ResultCode { get; set; }
		public string ResultMessage { get; set; }
		public bool Successful { get; set; }
		public string PayUReference { get; set; }
	}
}