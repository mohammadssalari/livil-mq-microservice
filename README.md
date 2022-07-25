# Prototype Synopsis Microservice 

## Problem

As part of a PoC, we need to interface with a remote API that provides synopses for email content.

The solution should fit into an exising API microservice stack.

## Specification

The solution should ideally do the following:
- Connect to the MQ (optional)
- Listen for a request on the correct channel (e.g. `get_synopsis`)
- Parse the incoming JSON payload
- Pass the incoming content to the remote API (mock provided)
- Asynchronously respond with the JSON result by passing into the MQ (optional) or displaying it in the terminal.

## Deliverables

- The code so we can test in our own machine.
- Brief write-up of decisions taken, and reasoning behind them.


## Message Queue

The data would arrive (and the response would be sent) via message queue (e.g. RabbitMQ) with a specific exchange/queue label. 

The incoming message could be mocked (e.g. input read from a file, output written to the terminal) or demostrated with RabbitMQ using a couple of scripts as described [here](https://www.rabbitmq.com/tutorials/tutorial-three-go.html).

An example queue name could look like: `get_synopses`

## Input

The input sent to the MQ could look like the following: 

```json
{
  "reply_queue":"reply_b87091b9-f3cf-4ae7-ae27-bcfdd8111v11",
  "content":"I know this is the final release but can we add more features? can you rework to make the pizza look more delicious what is a hamburger menu or we don't need a backup, it never goes down!. Can you make pink a little more pinkish give us a complimentary logo along with the website, or I think we need to start from scratch, yet we are a startup concept is bang on, but can we look at a better execution I have printed it out, but the animated gif is not moving, but I know somebody who can do this for a reasonable cost. In an ideal world start on it today and we will talk about what i want next time and we need to make the new version clean and sexy do less with more I like it, but can the snow look a little warmer that's great, but we need to add this 2000 line essay. Mmm, exactly like that, but different make it original, yet make it sexy, for we have big contacts we will promote you that's great, but we need to add this 2000 line essay nor can we try some other colours maybe. Mmm, exactly like that, but different we are a non-profit organization. Low resolution? It looks ok on my screen can you make it look more designed can you remove my double chin on my business card photo? i don't like the way it looks could you solutionize that for me we have big contacts we will promote you. Can you make it more infographic-y make it sexy, and can you make the blue bluer?, can you make it stand out more?. Can you make the font bigger? can you lower the price for the website? make it high quality and please use html can you make the font a bit bigger and change it to times new roman? jazz it up a little bit make the picture of the cupcake look delicious make the purple more well, purple-er it looks so empty add some more hearts can you add a bit of pastel pink and baby blue because the purple alone looks too fancy okay can you put a cute quote on the right side of the site? oh no it looks messed up! i think we need to start from scratch jazz it up a little. Can it handle a million in one go make it pop, yet i need this to work in internet explorer!, nor this red is too red, i think this should be fairly easy so if you just want to have a look. Can you put \"find us on facebook\" by the facebook logo?"
}
```

## Output

The output expected to be sent to the reply queue could look like this:

```json
{
  "synopsis":"i need this to work in internet explorer"
}
```

