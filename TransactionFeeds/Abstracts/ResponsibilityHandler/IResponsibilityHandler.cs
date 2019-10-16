using System.ComponentModel;

namespace Abstracts
{
    public interface IResponsibilityHandler
    {
        IResponsibilityHandler SetNext(IResponsibilityHandler handler);

        object Handle(object request);
        event PropertyChangedEventHandler PropertyChanged;
    }
}
