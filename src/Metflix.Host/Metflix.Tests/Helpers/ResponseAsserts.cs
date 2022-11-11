using FluentAssertions;
using Metflix.Models.Responses;

namespace Metflix.Tests.Helpers
{
    public static class ResponseAsserts
    {
        public static void AssertStatusCode204WithEmptyModelAndMessageResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Null(actual.Model);
            Assert.Null(actual.Message);
            AssertResponseEquality(expected, actual);
        }

        public static void AssertStatusCode404WithEmptyModelResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Null(actual.Model);
            AssertResponseEquality(expected, actual);
        }

        public static void AssertStatusCode200Or201WithEmptyMessageResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Null(actual.Message);
            AssertResponseEquality(expected, actual);
        }

        public static void AssertStatusCode400WithEmptyModelResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Null(actual.Model);
            AssertResponseEquality(expected, actual);
        }

        public static void AssertStatusCode500WithEmptyModelResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Null(actual.Model);
            AssertResponseEquality(expected, actual);
        }

        public static void AssertResponseEquality<T>(BaseResponse<T> expected, BaseResponse<T> actual)
        {
            Assert.Equal(expected.HttpStatusCode, actual.HttpStatusCode);
            expected.Model.Should().BeEquivalentTo(actual.Model);
            Assert.Equal(expected.Message, actual.Message);
        }
    }
}
