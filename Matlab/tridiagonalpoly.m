function polert = tridiagonalpoly(b, a, g,x)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
    function eredmeny =  belso(b2, a2, g2,x2,k)
        if k==1
           eredmeny = a(1)-x2(1);
        elseif k==0
           eredmeny = 1;
        elseif k==2
            eredmeny = (a2(k)-x2(1))*(belso(b2(1,1:k-1), a2(1,1:k-1), g2(1,1:k-1),x2,k-1)) - ...
        b(k-1)*g(k-1)* ( 1 );
        else
        eredmeny = (a2(k)-x2(1))*(belso(b2(1,1:k-1), a2(1,1:k-1), g2(1,1:k-1),x2,k-1)) - ...
        b(k-1)*g(k-1)* (belso(b2(1,1:k-2), a2(1,1:k-2), g2(1,1:k-2),x2,k-2));
        end
    end
if (size(b,2)+1 ~= size(a,2)) | (size(g,2)+1 ~= size(a,2))
    error("diagonálisok nem megfelelöek")
end

n = size(x,1);

polert = zeros(1,n);
for i =1:n
    polert(i)=belso(b,a,g,x(i),size(a));
end
end