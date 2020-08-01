namespace Potter.Characters.Application.DTOs
{
    public class DefaultResult<T> : DefaultResultMessage where T : class
    {
        public T Data { get; private set; }

        public void SetData(T data) { Data = data; }
    }
}
