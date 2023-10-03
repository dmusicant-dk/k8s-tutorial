# Kubernetes Workshop

This is a Workshop of some of the main concepts in kubernetes, it will be followed by a Helm Workshop. It covers the following functionality:

1. Multiple namespaces (and example of communicating across them)
1. A C# REST service
1. A MySQL Database
1. Secrets
1. Load balancer services
1. Ingress (to reach it via a url like `draftkingsk8s.com`)

It can be run locally on your laptop _(we use minikube for the Workshop and have install instructions)_.

## Workshop

The actual workshop steps are located at [the DraftKings Blog](https://medium.com/draftkings-engineering)

## Source Code

This contains the source code for the Workshop in two parts:

1. `/SampleRest` holds the C# application and Dockerfile that we'll use in the Workshop
1. `/kubernetes-yamls` holds all the yamls we create in the Workshop