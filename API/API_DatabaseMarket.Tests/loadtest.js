import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    vus: 20,           // виртуальные пользователи
    duration: '30s',   // длительность теста
};

export default function () {
    http.get("https://localhost:7166/api/v1/OrderItems");
    sleep(1);
}
