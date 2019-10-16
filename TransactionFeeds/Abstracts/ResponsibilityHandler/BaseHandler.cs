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

        protected abstract object ImplementHandle(object request);

        public object Handle(object request)
        {
            var result = ImplementHandle(request);
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
