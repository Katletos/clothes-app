using System.Collections;

namespace UnitTests;

public abstract class TestDataBase<TTestCase> : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return GetTestData().Select(d => new object[] { d }).GetEnumerator();
    }

    protected abstract IEnumerable<TTestCase> GetTestData();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}