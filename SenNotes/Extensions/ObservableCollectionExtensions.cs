using System.Collections.ObjectModel;

namespace SenNotes.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sorted = collection.ToList();
            sorted.Sort(comparison);

            for (int i = 0; i < sorted.Count; i++)
            {
                if (!Equals(collection[i], sorted[i]))
                {
                    collection.Move(collection.IndexOf(sorted[i]), i);
                }
            }
        }
    }
}