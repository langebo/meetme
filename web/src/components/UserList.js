import React, { useState, useEffect } from 'react'
import { withAuth } from '@okta/okta-react';

const UserList = () => {
    const [users, setUsers] = useState([])

    useEffect(() => {
        const token = JSON.parse(localStorage.getItem('okta-token-storage')).accessToken.accessToken
        fetch("http://localhost:5000/users", { headers: {'Authorization': "Bearer " + token}})
            .then(response => response.json())
            .then(json => json.map(entry => {
                let user = { 
                    id: entry.id,
                    name: entry.name,
                    email: entry.email 
                }
                return user;
            }))
            .then(usrs => setUsers(usrs))
    }, [])

    return (
        <ul>
            {users.map(u => <li key={u.id}>{u.name} ({u.email})</li>)}
        </ul>
    )
}

export default withAuth(UserList)