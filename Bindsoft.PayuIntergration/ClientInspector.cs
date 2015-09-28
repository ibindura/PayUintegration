using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Bindsoft.PayuIntergration
{
	public class ClientInspector : IClientMessageInspector
	{
		public MessageHeader[] Headers { get; set; }
		public ClientInspector(params MessageHeader[] headers)
		{
			Headers = headers;
		}
		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			if (Headers != null)
			{
				for (int i = Headers.Length - 1; i >= 0; i--)
					request.Headers.Insert(0, Headers[i]);
			}
			return request;
		}
		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

	}
}