import React from 'react';
import {
  Icon,
  Stack,
  Text,
  Facepile,
  OverflowButtonType,
  IconButton,
} from 'office-ui-fabric-react';
import { Card } from '@uifabric/react-cards';
import styled from 'styled-components';

import generateInitials from '../utils/initials';

const Meeting = props => {
  const { id, title, proposals, invitations } = props;

  const personas = invitations.map(invite => {
    return {
      text: invite.user.name,
      secondaryText: invite.user.email,
      imageInitials: generateInitials(invite.user.name),
      initialsColor: `#${invite.user.id.substring(0, 6)}`,
    };
  });

  const facepileProps = {
    personas: personas,
    maxDisplayablePersonas: 3,
    overflowButtonType: OverflowButtonType.descriptive,
    overflowButtonProps: {
      onClick: ev => alert('overflow icon clicked'),
    },
  };

  const onMoreClicked = e => {
    console.log('more clicked for meeting ' + id);
    console.log(e);
  };

  const getProposalsCount = () => proposals.length;

  const getVotesCount = () =>
    invitations.map(invite => invite.votes.length).reduce((prev, cur) => prev + cur);

  return (
    <Card compact>
      <Card.Item>
        <Icon iconName="SearchCalendar" styles={headIconStyle} />
      </Card.Item>
      <Card.Section fill styles={{ root: { width: 375 } }}>
        <CardContentContainer>
          <CardContentTitle>{title}</CardContentTitle>
          <CardContentStats>
            <Stack>
              <Text variant="small">
                <b>{getProposalsCount()}</b> proposals
              </Text>
              <Text variant="small">
                <b>{getVotesCount()}</b> votes
              </Text>
            </Stack>
          </CardContentStats>
          <CardContentInvites>
            <Facepile {...facepileProps} />
          </CardContentInvites>
        </CardContentContainer>
      </Card.Section>
      <Card.Section styles={{ root: { padding: '24px 0px' } }}>
        <IconButton iconProps={iconButtonProps} onClick={onMoreClicked}></IconButton>
      </Card.Section>
    </Card>
  );
};

export default Meeting;

const CardContentContainer = styled.div`
  height: 64px;
  padding-top: 8px;
  padding-bottom: 8px;
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: auto 1fr;
  grid-template-areas:
    'title title'
    'stats invites';
`;

const CardContentTitle = styled.span`
  grid-area: title;
  font-size: 14px;
  font-weight: 500;
`;

const CardContentStats = styled.div`
  grid-area: stats;
  display: flex;
  align-items: flex-end;
`;

const CardContentInvites = styled.div`
  grid-area: invites;
  display: flex;
  justify-content: flex-end;
  align-items: flex-end;
`;
const headIconStyle = {
  root: {
    color: '#2980b9',
    fontSize: 48,
    padding: 16,
  },
};

const iconButtonProps = {
  iconName: 'MoreVertical',
  styles: {
    root: {
      color: '#2980b9',
      fontSize: 20,
    },
  },
};
