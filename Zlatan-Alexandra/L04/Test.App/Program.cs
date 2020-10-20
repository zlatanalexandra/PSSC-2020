﻿using Profile.Domain.CreateProfileWorkflow;
using System;
using System.Collections.Generic;
using System.Net;
using static Profile.Domain.CreateProfileWorkflow.CreateProfileResult;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new CreateProfileCmd("Alexandra", string.Empty, "Alexandra", "alexandra.zlatan@test.com");
            var result = CreateProfile(cmd);

            result.Match(
                    ProcessProfileCreated,
                    ProcessProfileNotCreated,
                    ProcessInvalidProfile
                );

            Console.ReadLine();
        }

        private static ICreateProfileResult ProcessInvalidProfile(ProfileValidationFailed validationErrors)
        {
            Console.WriteLine("Validare de profil esuata: ");
            foreach (var error in validationErrors.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            return validationErrors;
        }

        private static ICreateProfileResult ProcessProfileNotCreated(ProfileNotCreated profileNotCreatedResult)
        {
            Console.WriteLine($"Profil necreat: {profileNotCreatedResult.Reason}");
            return profileNotCreatedResult;
        }

        private static ICreateProfileResult ProcessProfileCreated(ProfileCreated profile)
        {
            Console.WriteLine($"Profil {profile.ProfileId}");
            return profile;
        }

        public static ICreateProfileResult CreateProfile(CreateProfileCmd createProfileCommand)
        {
            if (string.IsNullOrWhiteSpace(createProfileCommand.EmailAddress))
            {
                var errors = new List<string>() { "Adresa invalida" };
                return new ProfileValidationFailed(errors);
            }

            if(new Random().Next(10) > 1)
            {
                return new ProfileNotCreated("Email-ul nu a putut fi verificat");
            }

            var profileId = Guid.NewGuid();
            var result = new ProfileCreated(profileId, createProfileCommand.EmailAddress);

            //execute logic
            return result;
        }
    }
}
