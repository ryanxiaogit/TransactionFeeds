namespace Abstracts.ResponsibilityHandler
{
    public abstract class BaseHandler : IResponsibilityHandler
    {
        private IResponsibilityHandler _nextHandler;

        public IResponsibilityHandler SetNext(IResponsibilityHandler handler)
        {
            this._nextHandler = handler;

            return handler;
        }

        protected abstract object ActualHandle(object request);

        public object Handle(object request)
        {
            var result = ActualHandle(request);
            if (result == null)
            {
                if (this._nextHandler != null)
                {
                    return this._nextHandler.Handle(request);
                }
            }
            return result;
        }
    }
}
