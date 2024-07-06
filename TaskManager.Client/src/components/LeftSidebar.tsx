import { Drawer, List, ListItemButton, ListItemText, Toolbar, Box } from '@mui/material';
import { useRouter } from 'next/navigation';

interface LeftSidebarProps {
  open: boolean;
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

export default function LeftSidebar({ open }: LeftSidebarProps): React.ReactElement {
  const router = useRouter();

  return (
    <Drawer variant="persistent" open={open}>
      <Toolbar />
      <Box >
        <List>
          {menuRoutes.map((route: Route) => (
            <ListItemButton key={route.name} onClick={() => router.push(route.path)}>
              <ListItemText primary={route.name}/>
            </ListItemButton>
          ))}
        </List>
      </Box>
    </Drawer>
  );
}
