FROM node:22-alpine
WORKDIR /message-chat

COPY package.json ./

RUN yarn install --frozen-lockfile

COPY . .

EXPOSE 3000

CMD ["yarn", "start"]