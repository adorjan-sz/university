function polert = tridiagonalpoly(b, a, g,x)
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
    function eredmeny =  belso(b2, a2, g2,x2,k)
        if k==1
           eredmeny = a2(1)-x2(1);
        
        elseif k==2
            resz1 = belso(b2(1,1:k-1), a2(1,1:k-1), g2(1,1:k-1),x2,k-1);
            eredmeny = (a2(k)-x2(1))*(resz1) - ...
            b2(k-1)*g2(k-1)* ( 1 );
        else
            resz1 = belso(b2(1,1:k-1), a2(1,1:k-1), g2(1,1:k-1),x2,k-1);
            resz2 = belso(b2(1,1:k-2), a2(1,1:k-2), g2(1,1:k-2),x2,k-2);
            eredmeny = (a2(k)-x2(1))*(resz1) - ...
            b2(k-1)*g2(k-1)* (resz2);
        end
    end
if (size(b,2)+1 ~= size(a,2)) | (size(g,2)+1 ~= size(a,2))
    error("diagonálisok nem megfelelöek")
end

n = size(x,1);

polert = zeros(1,n);
for i =1:n
    polert(i)=belso(b,a,g,x(i),length(a));
end
end