import { useParams, useRouter } from 'next/navigation';

import {
  Box,
  Divider,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar
} from '@mui/material';


import BoardIcon from '@mui/icons-material/ViewKanbanOutlined'
import CloseIcon from '@mui/icons-material/Close'
import HomeIcon from '@mui/icons-material/HomeOutlined'
import TasksIcon from '@mui/icons-material/ListAltOutlined'

interface LeftSidebarProps {
  open: boolean;
  onClose: () => void;
}

interface MenuItem {
  icon: React.ReactElement;
  name: string;
  route: string;
}

const menuRoutes: MenuItem[] = [
  {
    icon: <HomeIcon/>,
    name: 'Home',
    route: '/spaces/[spaceKey]/dashboard'
  },
  {
    icon: <BoardIcon/>,
    name: 'Board',
    route: '/spaces/[spaceKey]/board'
  },
  {
    icon: <TasksIcon/>,
    name: 'Tasks',
    route: '/spaces/[spaceKey]/tasks'
  },
];

export default function LeftSidebar({ open, onClose }: LeftSidebarProps): React.ReactElement {
  const router = useRouter();
  const params = useParams<{ spaceKey: string; }>();

  return (
    <Drawer
      variant='temporary'
      open={open}
      onClose={() => onClose()}
      sx={{
        '& .MuiDrawer-paper': {
          borderTopRightRadius: '8px',
          borderBottomRightRadius: '8px',
          width: '20rem'
        }
      }}
    >
      <Toolbar>
        <Box sx={{flexGrow: 1}}/>
        <IconButton onClick={() => onClose()}>
          <CloseIcon/>
        </IconButton>
      </Toolbar>
      <Box >
        <List>
          {menuRoutes.map((menuItem: MenuItem) => (
            <ListItem disablePadding>
              <ListItemButton
                key={menuItem.name}
                disabled={!params.spaceKey}
                onClick={() => {
                  router.push(menuItem.route.replace('[spaceKey]', params.spaceKey));
                  onClose();
                }}
              >
                <ListItemIcon>
                  {menuItem.icon}
                </ListItemIcon>
                <ListItemText primary={menuItem.name}/>
              </ListItemButton>
            </ListItem>
          ))}
          <Divider/>
        </List>
      </Box>
    </Drawer>
  );
}
