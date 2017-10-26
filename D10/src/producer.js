import createRabbitClient from "./createRabbitClient";

export default topic =>
  new Promise((resolve, reject) => {
    createRabbitClient().then(ch => {
      ch.assertQueue(topic).then(ok => {
        resolve(ch);
      });
    });
  });
