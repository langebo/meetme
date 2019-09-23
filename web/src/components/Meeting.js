import React from 'react';
import { Icon, Text, Facepile, OverflowButtonType } from 'office-ui-fabric-react'
import { FontWeights } from '@uifabric/styling';
import { Card } from '@uifabric/react-cards'

import generateInitials from '../utils/initials'

const Meeting = props => {

  const headIconStyle = {
    root: {
      color: '#b71540',
      fontSize: 48,
      padding: 18
    }
  }

  const buttonIconStyle = {
    root: {
      color: '#b71540',
      fontSize: 18
    }
  }

  const textStyles = {
    root: {
      paddingTop: 12,
      fontWeight: FontWeights.semibold
    }
  }

  const cardTokens = {
    childrenMargin: 12
  }

  const mainSectionStyles = {
    root: {
      width: 350,
    }
  };

  const buttonSectionTokens = {
    padding: '0px 0px 0px 12px'
  };

  const buttonSectionStyles = {
    root: {
      borderLeft: '1px solid #F3F2F1'
    }
  };

  const personas = props.invitations.map(invite => {
    const persona = {
      text: invite.user.name,
      secondaryText: invite.user.email,
      imageInitials: generateInitials(invite.user.name),
      initialsColor: `#${invite.user.id.substring(0, 6)}`
    }

    return persona;
  })

  const facepileProps = {
    personas: personas,
    maxDisplayablePersonas: 3,
    overflowButtonType: OverflowButtonType.descriptive,
    overflowButtonProps: {

      onClick: (ev) => alert('overflow icon clicked')
    },
    ariaDescription: 'To move through the items use left and right arrow keys.'
  };

  return (
    <Card compact tokens={cardTokens}>
      <Card.Item>
        <Icon iconName="SearchCalendar" styles={headIconStyle} />
      </Card.Item>
      <Card.Section fill styles={mainSectionStyles}>
        <Text variant="small" styles={textStyles}>{props.title}</Text>
        <Facepile {...facepileProps}></Facepile>
      </Card.Section>
      <Card.Section styles={buttonSectionStyles} tokens={buttonSectionTokens}>
        <Icon iconName="CheckboxComposite" styles={buttonIconStyle} />
        <Icon iconName="Edit" styles={buttonIconStyle} />
        <Icon iconName="MoreVertical" styles={buttonIconStyle} />
      </Card.Section>
    </Card>
  );
};

export default Meeting;
