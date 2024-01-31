using RandomUtility;

namespace ArrayUtility
{
    public static class ArrayOperations
    {
        /// <summary>
        /// Fills an array with random numbers within the specified range.
        /// </summary>
        /// <param name="array">The array to fill.</param>
        /// <param name="from">The lower bound of the random numbers.</param>
        /// <param name="to">The upper bound of the random numbers.</param>
        public static void FillArrayWithRandomNumbers(int[] array, int from = 0, int to = 100)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = RandomGenerator.RandomNumber(from, to);
            }
        }

        // Function to shuffle an array of integers
        public static void ShuffleArray(int[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomGenerator.RandomNumber(0, length - 1);
                int randomIndex2 = RandomGenerator.RandomNumber(0, length - 1);
                Swap(arr, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle an array of strings
        public static void ShuffleArray(string[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomGenerator.RandomNumber(0, length - 1);
                int randomIndex2 = RandomGenerator.RandomNumber(0, length - 1);
                Swap(arr, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle a List of integers
        public static void ShuffleArray(List<int> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomGenerator.RandomNumber(0, length - 1);
                int randomIndex2 = RandomGenerator.RandomNumber(0, length - 1);
                Swap(list, randomIndex1, randomIndex2);
            }
        }

        // Function to shuffle a List of strings
        public static void ShuffleArray(List<string> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                int randomIndex1 = RandomGenerator.RandomNumber(0, length - 1);
                int randomIndex2 = RandomGenerator.RandomNumber(0, length - 1);
                Swap(list, randomIndex1, randomIndex2);
            }
        }

        // Function to swap elements at two indices in a list
        private static void Swap<T>(List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        // Function to swap elements at two indices in an array
        private static void Swap<T>(T[] arr, int index1, int index2)
        {
            T temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }

        // Function to check if an array of integers is a palindrome
        public static bool IsPalindromeArray(int[] arr)
        {
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if an array of doubles is a palindrome
        public static bool IsPalindromeArray(double[] arr)
        {
            int length = arr.Length;

            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if an array of strings is a palindrome
        public static bool IsPalindromeArray(string[] arr)
        {
            int length = arr.Length;

            for (int i = 0; i < length; i++)
            {
                if (arr[i] != arr[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of integers is a palindrome
        public static bool IsPalindromeArray(List<int> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of doubles is a palindrome
        public static bool IsPalindromeArray(List<double> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }

        // Function to check if a List of strings is a palindrome
        public static bool IsPalindromeArray(List<string> list)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (list[i] != list[length - i - 1])
                    return false;
            }
            return true;
        }
    }
}
