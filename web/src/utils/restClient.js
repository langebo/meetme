import axios from 'axios';

const restClient = token =>
  axios.create({
    baseURL: 'http://localhost:5000',
    timeout: 5000,
    headers: { Authorization: 'Bearer ' + token },
  });

export default restClient;
//\~8IpS8AvrD;SkMg/yCs
