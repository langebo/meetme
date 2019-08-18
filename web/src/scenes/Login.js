import React, { useState, useEffect } from 'react'
import { withAuth } from '@okta/okta-react'
import { CompoundButton } from 'office-ui-fabric-react'
import 'office-ui-fabric-react/dist/css/fabric.css';

const Login = props => {
    const [authenticated, setAuthenticated] = useState(false)

    const login = async () => props.auth.login('/dashboard')
    
    useEffect(() => {
        let checkAuthentication = async () => {
            let authed = await props.auth.isAuthenticated()
            if (authed !== authenticated)
                setAuthenticated(authed);
        }
        checkAuthentication()
    }, [authenticated, props])

    const divStyle = {
        paddingTop: 200
    }

    return (
        <div style={divStyle}>
            <div className="ms-Grid" dir="ltr">
                <div className="ms-Grid-row">
                    <div className="ms-Grid-col ms-sm6 ms-md4 ms-lg3"/>
                    <div className="ms-Grid-col ms-sm6 ms-md8 ms-lg9">
                        <h2>MeetMe</h2>
                        <h3>The best way to schedule meetings</h3>
                    </div>
                </div>
                    <div className="ms-Grid-row">
                    <div className="ms-Grid-col ms-sm4 ms-md5 ms-lg5"/>
                    <div className="ms-Grid-col ms-sm4 ms-md4 ms-lg4">
                        <CompoundButton onClick={login} primary={false} secondaryText="Via Microsoft or Google">Login</CompoundButton>
                    </div>
                    <div className="ms-Grid-col ms-sm4 ms-md3 ms-lg3"/>
                </div>
            </div>
        </div>
    )
}

export default withAuth(Login)