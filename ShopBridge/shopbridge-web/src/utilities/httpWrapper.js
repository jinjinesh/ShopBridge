import axios from 'axios';

const instance = axios.create();
export default {
    get (url) {
        return instance.get(url);
    },
    post (url, data) {
        return instance.post(url, data);
    },
    put (url, data) {
        return instance.put(url, data);
    },
    delete (url) {
        return instance.delete(url);
    }
};
