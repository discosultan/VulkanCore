using System;
using System.Diagnostics;

namespace VulkanCore
{
    /// <summary>
    /// Represents an error from executing a Vulkan command.
    /// </summary>
    public class VulkanException : Exception
    {
        private const string DefaultMessage = "Vulkan command failed to execute.";

        /// <summary>
        /// Initializes a new instance of the <see cref="VulkanException"/> class.
        /// </summary>
        /// <param name="result">The result returned by Vulkan.</param>
        /// <param name="message">The message that describes the error.</param>
        public VulkanException(Result result, string message = DefaultMessage)
            : base($"[{(int)result}] {result} - {message}")
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result returned by Vulkan.
        /// </summary>
        public Result Result { get; }

        /// <summary>
        /// Gets if the result is considered an error.
        /// </summary>
        public bool IsError => Result < 0;

        [DebuggerHidden]
        [DebuggerStepThrough]
        internal static void ThrowForInvalidResult(Result result)
        {
            if (result != Result.Success)
                throw new VulkanException(result);
        }
    }
}
