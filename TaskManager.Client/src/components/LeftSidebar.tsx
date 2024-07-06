import {
  Box,
  Drawer,
  IconButton,
  List,
  ListItemButton,
  ListItemText,
  Toolbar
} from '@mui/material';
import { useRouter } from 'next/navigation';

import CloseIcon from '@mui/icons-material/Close'

interface LeftSidebarProps {
  open: boolean;
  onClose: () => void;
}

interface Route {
  name: string;
  path: string;
}

const menuRoutes: Route[] = [
  { name: 'Home', path: '/' },
  { name: 'Board', path: '/board' },
  { name: 'Tasks', path: '/tasks' }
];

export default function LeftSidebar({ open, onClose }: LeftSidebarProps): React.ReactElement {
  const router = useRouter();

  return (
    <Drawer
      variant='temporary'
      open={open}
      onClose={() => onClose()}
      sx={{
        '& .MuiDrawer-paper': {
          borderTopRightRadius: '8px',
          borderBottomRightRadius: '8px'
        }
      }}
    >
      <Toolbar>
        <IconButton onClick={() => onClose()}>
          <CloseIcon/>
        </IconButton>
      </Toolbar>
      <Box >
        <List>
          {menuRoutes.map((route: Route) => (
            <ListItemButton
              key={route.name}
              onClick={() => {
                router.push(route.path);
                onClose();
              }}
            >
              <ListItemText primary={route.name}/>
            </ListItemButton>
          ))}
        </List>
      </Box>
    </Drawer>
  );
}
