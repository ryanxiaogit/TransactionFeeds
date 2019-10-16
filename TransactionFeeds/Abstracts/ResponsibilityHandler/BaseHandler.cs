using System.ComponentModel;

namespace Abstracts.ResponsibilityHandler
{
    public abstract class BaseHandler : IResponsibilityHandler
    {
        private IResponsibilityHandler _nextHandler;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isEndHere;

        public bool IsEndHere
        {
            get
            {
                return isEndHere;
            }
            protected set
            {
                isEndHere = value;
                OnPropertyChanged("IsEndHere");
            }
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
                IsEndHere = false;
                if (this._nextHandler != null)
                {
                    return this._nextHandler.Handle(request);
                }
            }
            else
            {
                IsEndHere = true;
            }
            return result;
        }
    }
}
