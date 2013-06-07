
using System.Linq;
using Controls.Interface;

namespace Controls.Collections
{
    public class ContentCollection<T> : ObservableList<T>
    {
        private readonly IContentHost<T> _host;

        public ContentCollection(IContentHost<T> host)
        {
            _host = host;
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            var olditems = (args.OldItems ?? new T[0]).OfType<T>();
            _host.Remove(olditems);
            var newitems = (args.NewItems ?? new T[0]).OfType<T>();
            _host.Add(newitems);
            _host.Update();
            base.OnCollectionChanged(args);
        }
    }
}
