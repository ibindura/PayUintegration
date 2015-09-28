using System.ServiceModel.Channels;
using System.Xml;

namespace Bindsoft.PayuIntergration
{
	public class SecurityHeader : MessageHeader
	{
		public string SystemUser { get; set; }
		public string SystemPassword { get; set; }
		public SecurityHeader(string systemUser, string systemPassword)
		{
			SystemUser = systemUser;
			SystemPassword = systemPassword;
		}
		public override string Name
		{
			get { return "Security"; }
		}
		public override string Namespace
		{
			get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
		}

		public override bool MustUnderstand
		{
			get { return true; }
		}

		protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
		{
			WriteHeader(writer);
		}
		private void WriteHeader(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement("wsse", "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
			writer.WriteStartElement("wsse", "Username", null);
			writer.WriteString(SystemUser);
			writer.WriteEndElement();//End Username 
			writer.WriteStartElement("wsse", "Password", null);
			writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
			writer.WriteString(SystemPassword);
			writer.WriteEndElement();//End Password 
			writer.WriteEndElement();//End UsernameToken
			writer.Flush();
		}
	}
}