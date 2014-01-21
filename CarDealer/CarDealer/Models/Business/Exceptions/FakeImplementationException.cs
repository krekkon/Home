using System;

namespace CarDealerProject.Models.Business.Exceptions
{
    public class FakeImplementationException : Exception
    {
        public FakeImplementationException() : base("This exception is expected and fired because of mocking dependencies")
        {
            //TODO check and verify the logic and working of this
        }
    }
}