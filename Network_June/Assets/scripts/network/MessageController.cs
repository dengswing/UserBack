namespace com.shinezone.network
{
    /// <summary>
    /// 委托控制器
    /// </summary>
    public class MessageController : SingleInstance<MessageController>
    {
        public enum TouchType
        {
            SOCKET
        }

        private SocketResultDelegate socketBack;

        public void AddListenerScoketResult(SocketResultDelegate socketBack)
        {
            this.socketBack = socketBack;
        }

        public void RemoveListenerScoketResult()
        {
            this.socketBack = null;
        }

        public void Touch(TouchType cmd)
        {
            switch (cmd)
            {


            }

        }

    }
}