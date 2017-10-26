import createRabbitClient from "./createRabbitClient";
import { userTopic } from "./mqTopics";

createRabbitClient().then(ch =>
  ch.assertQueue(userTopic).then(ok => {
    return ch.consume(userTopic, msg => {
      if (msg !== null) {
        const data = JSON.parse(msg.content.toString());
        console.log(
          "User created with \nid:",
          data.id,
          "\nname:",
          data.name,
          "\ngender:",
          data.gender
        );
        ch.ack(msg);
      }
    });
  })
);
