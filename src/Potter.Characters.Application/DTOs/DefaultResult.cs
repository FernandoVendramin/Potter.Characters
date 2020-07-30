using System.Collections.Generic;

namespace Potter.Characters.Application.DTOs
{
    public class DefaultResult<T> where T : class
    {
        public DefaultResult()
        {
            Success = false;
            Messages = new List<string>();
        }

        public DefaultResult(bool success)
        {
            Success = success;
            Messages = new List<string>();
        }

        public bool Success { get; private set; }
        public T Data { get; private set; }
        public List<string> Messages { get; private set; }

        public void SetData(T data) { Data = data; Success = Messages.Count == 0; }
        public void IsSuccess() { Success = true; }
        public void IsNotSuccess() { Success = false; }
        public void SetMessage(string message) { Messages.Add(message); }
        public void SetMessages(List<string> messages) { Messages.AddRange(messages); }
        public void ClearMessages() { Messages.Clear(); }
    }
}
