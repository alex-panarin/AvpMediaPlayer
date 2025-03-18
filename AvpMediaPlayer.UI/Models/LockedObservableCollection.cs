using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AvpMediaPlayer.UI.Models
{
    public class LockedObservableCollection<TEntity> : ObservableCollection<TEntity>
    {
        class Locker : IDisposable
        {
            private readonly LockedObservableCollection<TEntity> _collection;
            public Locker(LockedObservableCollection<TEntity> collection)
            {
                _collection = collection;
                _collection.LockRaiseEvent = true;
            }
            public void Dispose()
            {
                _collection.LockRaiseEvent = false;
                _collection.RaiseCollectionChanged();
            }
        }

        private bool LockRaiseEvent { get; set; } = false;
        private void RaiseCollectionChanged()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<TEntity> collection)
        {
            if (collection == null) return;

            using (IEnumerator<TEntity> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    this.Add(enumerator.Current);
                }
            }
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (LockRaiseEvent) return;
            base.OnCollectionChanged(e);
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (LockRaiseEvent) return;
            base.OnPropertyChanged(e);
        }
        public IDisposable LockChangedEvent()
        {
            return new Locker(this);
        }
    }
}
