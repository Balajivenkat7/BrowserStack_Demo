//Created By: Balaji  Venkatesan
//Created Date: 26 Sep 2025
using NUnit.Framework;

// Run different Test methods in parallel. Keep fixture sequential unless explicitly opted-in.
[assembly: Parallelizable(ParallelScope.Children)]

// Set the maximum number of concurrent tests. Adjust to match your BrowserStack concurrency.
// Example: 4 parallel workers. Increase/decrease as per your plan/account.
[assembly: LevelOfParallelism(4)]

//Create single instance for each and every test method to avoid state leak between test while running in parallel.
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]