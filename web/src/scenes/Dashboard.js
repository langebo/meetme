import React, { useEffect, useState } from 'react'
import { withAuth } from '@okta/okta-react'
import UserList from '../components/UserList'

const Dashboard = props => {
    const [user, setUser] = useState({})
    useEffect(() => {
        props.auth.getUser()
            .then(result => setUser(result));
    }, [props])
    
    return (
        <div>
            <h1>Dashboard</h1>
            <h2>welcome {user.name}</h2>
            <UserList/>
        </div>
    );
}

export default withAuth(Dashboard)