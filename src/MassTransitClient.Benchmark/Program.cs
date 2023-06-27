using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using MassTransitClient.Benchmark;

var summary = BenchmarkRunner.Run<MassTransitClientBenchmark>(DefaultConfig.Instance.AddDiagnoser());