using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;

var typeMap = new Dictionary<string, Type>
{
    { "v1/Pod", typeof(V1Pod) },
    { "v1/Service", typeof(V1Service) },
    { "apps/v1/Deployment", typeof(V1Deployment) },
};

var objects = await KubernetesYaml.LoadAllFromFileAsync(args[0], typeMap).ConfigureAwait(false);

foreach (var obj in objects)
{
    Console.WriteLine(obj);
}
