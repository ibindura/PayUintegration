namespace Bindsoft.PayuIntergration
{
	public class GetTransactionResult
	{
		public bool IsTransactionSuccessfull { get; set; }
		public string TransactionState { get; set; }
		public string PayUReference { get; set; }
		public string DisplayMessage { get; set; }
		public string ErrorMessage { get; set; }
		public string ResultCode { get; set; }
		public string RequestTrace { get; set; }
	}
}