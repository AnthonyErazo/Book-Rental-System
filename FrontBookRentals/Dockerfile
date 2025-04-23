FROM node:18-alpine as build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build --prod
RUN ls -la /app/dist/front-book-rentals

FROM nginx:alpine
RUN ls -la /usr/share/nginx/html
COPY --from=build /app/dist/front-book-rentals/browser /usr/share/nginx/html
RUN ls -la /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN echo "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAABUSURBVDiNY2AYBaNgFIyCUTAKRsEoGAWjYBSMglEwCkbBKBgFo2AUjIJRMApGwSgYBaNgFIyCUTAKRsEoGAWjYBaNgFIyCUTAKRsEoGAWjYBSMglEwCkbBKBgFo2AUjIJRMApGAQAzVQYFQYQYJAAAAABJRU5ErkJggg==" | base64 -d > /usr/share/nginx/html/favicon.ico
EXPOSE 4200
CMD ["nginx", "-g", "daemon off;"] 