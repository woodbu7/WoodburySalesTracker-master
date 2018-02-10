﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSalesTracker
{
    public static class ConsoleValidator
    {
        /// <summary>
        /// helper method to get a valid integer from the user within a range
        /// </summary>
        /// <param name="minValue">inclusive minimum value</param>
        /// <param name="maxValue">inclusive maximum value</param>
        /// <param name="maxAttempts">maximum number of attempts</param>
        /// <param name="pluralName">plural name of item</param>
        /// <param name="validInput">indicates valid user input</param>
        /// <returns></returns>
        public static bool TryGetIntegerFromUser(int minValue, int maxValue, int maxAttempts, string pluralName, out int userInteger)
        {
            bool validInput = false;
            bool maxAttemptsExceeded = false;
            string userResponse;
            string feedbackMessage = "";
            int attempts = 1;

            userInteger = 0;

            while (!validInput && !maxAttemptsExceeded)
            {
                //
                // more attempts available
                //
                if (attempts <= maxAttempts)
                {
                    ConsoleUtil.DisplayPromptMessage($"Enter the number, between {minValue} and {maxValue}, of {pluralName}:");
                    userResponse = Console.ReadLine();
                    ConsoleUtil.DisplayMessage("");

                    //
                    // input is an Integer
                    //
                    if (int.TryParse(userResponse, out userInteger))
                    {
                        //
                        // input is in range
                        //
                        if (userInteger >= minValue && userInteger <= maxValue)
                        {
                            validInput = true;
                        }
                        //
                        // input is not in range
                        //
                        else
                        {
                            feedbackMessage = $"The number {userInteger} is not in the specified range.";
                        }
                    }
                    //
                    // input is not an Integer
                    //
                    else
                    {
                        feedbackMessage = $"{userResponse} is not an integer.";
                    }

                    if (!validInput && attempts <= maxAttempts)
                    {
                        ConsoleUtil.DisplayMessage($"You entered: {userResponse}");
                        ConsoleUtil.DisplayMessage(feedbackMessage);

                        if (attempts < maxAttempts)
                        {
                            ConsoleUtil.DisplayMessage($"Please enter an integer between {minValue} and {maxValue}.");
                            ConsoleUtil.DisplayMessage("Press any key to try again.");
                            Console.ReadKey();
                        }
                        else
                        {
                            ConsoleUtil.DisplayMessage("It appears you have exceeded the maximum number of attempts allowed.");
                            ConsoleUtil.DisplayMessage("Press any key to continue.");
                            Console.ReadKey();
                        }

                        Console.Clear();
                    }
                    else
                    {
                        ConsoleUtil.DisplayMessage("");
                    }

                    attempts++;
                }
                else
                {
                    maxAttemptsExceeded = true;
                }
            }

            return validInput;
        }

        /// <summary>
        /// helper method to get a valid positive integer from the user
        /// </summary>
        /// <param name="maxAttempts">maximum number of attempts</param>
        /// <param name="userPrompt">user prompt</param>
        /// <param name="validInput">indicates valid user input</param>
        /// <returns></returns>
        public static string GetYesNoFromUser(int maxAttempts, string userPrompt, out bool maxAttemptsExceeded)
        {
            bool validInput = false;
            maxAttemptsExceeded = false;
            string userResponse = "";
            int attempts = 1;

            while (!validInput && !maxAttemptsExceeded)
            {
                Console.Write($"{userPrompt} [Yes / No] ");
                userResponse = Console.ReadLine().ToUpper();
                ConsoleUtil.DisplayMessage("");

                //
                // input is valid
                //
                if (userResponse == "YES" || userResponse == "NO")
                {
                    validInput = true;
                }
                //
                // input is invalid, but more attempts available
                //
                else
                {
                    ConsoleUtil.DisplayMessage($"You entered: {userResponse}");
                    ConsoleUtil.DisplayMessage($"\"{userResponse}\" is not a valid response.");

                    //
                    // more attempts available 
                    //
                    if (attempts < maxAttempts)
                    {
                        ConsoleUtil.DisplayMessage($"Please enter either \"Yes\" or \"No\".");
                        ConsoleUtil.DisplayMessage("Press any key to try again.");
                    }
                    //
                    // all attempts used
                    //
                    else
                    {
                        ConsoleUtil.DisplayMessage("It appears you have exceeded the maximum number of attempts allowed.");
                        ConsoleUtil.DisplayMessage("Press any key to continue.");
                        maxAttemptsExceeded = true;
                    }

                    Console.ReadKey();
                    Console.Clear();
                }

                attempts++;
            }

            return userResponse;
        }
    }
}
