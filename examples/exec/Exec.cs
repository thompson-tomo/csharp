using k8s;
using k8s.Models;
using System;
using System.Threading.Tasks;

var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
IKubernetes client = new Kubernetes(config);
Console.WriteLine("Starting Request!");

var list = client.CoreV1.ListNamespacedPod("default");
var pod = list.Items[0];
await ExecInPod(client, pod).ConfigureAwait(false);

async Task ExecInPod(IKubernetes client, V1Pod pod)
{
    var webSocket =
        await client.WebSocketNamespacedPodExecAsync(pod.Metadata.Name, "default", "ls",
            pod.Spec.Containers[0].Name).ConfigureAwait(false);

    var demux = new StreamDemuxer(webSocket);
    demux.Start();

    var buff = new byte[4096];
    var stream = demux.GetStream(1, 1);
    var read = stream.Read(buff, 0, 4096);
    var str = System.Text.Encoding.Default.GetString(buff);
    Console.WriteLine(str);
}
