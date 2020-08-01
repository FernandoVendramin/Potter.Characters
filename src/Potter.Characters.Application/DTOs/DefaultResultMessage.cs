using System.Collections.Generic;

namespace Potter.Characters.Application.DTOs
{
    public class DefaultResultMessage
    {
        public DefaultResultMessage()
        {
            Success = true;
            Messages = new List<string>();
        }

        public bool Success { get; private set; }
        public List<string> Messages { get; private set; }

        public void IsSuccess() { Success = true; }
        public void IsNotSuccess() { Success = false; }
        public void SetMessage(string message) { Messages.Add(message); Success = false; }
        public void SetMessages(List<string> messages) { Messages.AddRange(messages); Success = false; }
        public void ClearMessages() { Messages.Clear(); }
    }
}
