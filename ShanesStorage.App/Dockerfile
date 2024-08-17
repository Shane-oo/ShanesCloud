FROM node:20.12.2-alpine3.19 as build
WORKDIR /usr/local/app

COPY package*.json ./

RUN npm ci
RUN npm install
RUN npm install -g @angular/cli
COPY . .
RUN npm run build --prod

FROM nginx:alpine
COPY --from=build /usr/local/app/dist/shanes-storage.app/browser /usr/share/nginx/html
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

