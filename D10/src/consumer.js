import createRabbitClient from "./createRabbitClient";
import { userTopic, punchTopic, discountTopic } from "./mqTopics";

// UserConsumer
createRabbitClient().then(ch =>
  ch.assertQueue(userTopic).then(ok => {
    return ch.consume(userTopic, msg => {
      if (msg !== null) {
        const data = JSON.parse(msg.content.toString());
        console.log(
            "--------------------------------------",
          "\nUser was added with \nid:",
          data.id,
          "\nname:",
          data.name,
          "\ngender:",
          data.gender,
          "\ncreated:",
          data.created,
          "\n--------------------------------------"
        );
        ch.ack(msg);
      }
    });
  })
);
// PunchConsumer, when a punch is created
createRabbitClient().then(ch =>
  ch.assertQueue(punchTopic).then(ok => {
    return ch.consume(punchTopic, msg => {
      if (msg !== null) {
        const data = JSON.parse(msg.content.toString());
        console.log(
            "--------------------------------------",
          "\nUser got a punch with \nid:",
          data.user_id,
          "\nusers name:",
          data.user_name,
          "\nat company: \ncompany with id:",
          data.company_id,
          "\ncompany name:",
          data.company_name,
          "\nPunchCount:",
          data.punch_count,
          "\nDate of punch",
          data.created,
          "\nUnused punches for this user at this company:",
          data.unused_punches,
          "\n--------------------------------------"
        );
        ch.ack(msg);
      }
    });
  })
);
// PunchConsumer, when a discount is given
createRabbitClient().then(ch =>
  ch.assertQueue(discountTopic).then(ok => {
    return ch.consume(discountTopic, msg => {
      if (msg !== null) {
        const data = JSON.parse(msg.content.toString());
        console.log(
            "--------------------------------------",
            "\nUser got a discount - Punch count has been reached",
            "\nPunch created for user with \nid:",
            data.user_id,
            "\nusers name:",
            data.user_name,
            "\nat company with \nid:",
            data.company_id,
            "\ncompany name:",
            data.company_name,
            "\nPunchCount:",
            data.punch_count,
            "\nDate of punch",
            data.created,
            "\n--------------------------------------");
        ch.ack(msg);
      }
    });
  })
);
