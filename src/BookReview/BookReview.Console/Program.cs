// See https://aka.ms/new-console-template for more information
using BookReview.Service;

Console.WriteLine("Hello, World!");
var enc = new Encryptor();
var salt = enc.GenerateSalt();
var password = "Password";
var encPassword = enc.ComputeHash(password,salt);
Console.WriteLine($"Salt:{salt}. Length = {salt.Length}");
Console.WriteLine($"Enc Password:{encPassword}. Length = {encPassword.Length}");
