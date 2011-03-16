namespace Precio.Domain
{
    public class ValueObject
    {
    }

    public class ValueObject<T> : ValueObject
    {
        public static bool operator ==(ValueObject<T> v1, ValueObject<T> v2)
        {
            return v1.Compare(v2);
        }

        public static bool operator !=(ValueObject<T> v1, ValueObject<T> v2)
        {
            return !v1.Compare(v2);
        }

        public override int GetHashCode()
        {
            return this.ComputeHash();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public T Copy()
        {
            return (T) (object) this.Clone();
        }
    }
}