namespace CrudTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            DemoTestMath demo = new DemoTestMath();
            int input1 = 10, input2 = 20, expectedValue = 30;

            //Act
            int actialValue = demo.Add(input1, input2);

            //Assert
            Assert.Equal(expectedValue, actialValue);
        }
    }
}