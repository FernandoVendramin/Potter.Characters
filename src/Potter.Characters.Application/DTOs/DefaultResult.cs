using System.Collections.Generic;

namespace Potter.Characters.Application.DTOs
{
    public class DefaultResult<T> where T : class
    {
        public DefaultResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public bool Success { get; private set; }
        public T Data { get; private set; }
        public List<string> Messages { get; private set; }

        public void SetData(T data) { Data = data; }
        public void IsSuccess() { Success = true; }
        public void IsNotSuccess() { Success = false; }
        public void SetMessage(string message) { Messages.Add(message); Success = false; }
        public void SetMessages(List<string> messages) { Messages.AddRange(messages); Success = false; }
        public void ClearMessages() { Messages.Clear(); }
    }
}
